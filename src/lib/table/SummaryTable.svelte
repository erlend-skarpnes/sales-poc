<script lang="ts">
    import {Spinner, Table, TableBody, TableHead, TableHeadCell} from 'flowbite-svelte';
    import SummaryRow from "./SummaryRow.svelte";

    const endpoint = "http://localhost:5401/summaries";
    // let rows: DataRow[] = [{title: "Test", customer: "Test", role: "Test", summary: "Test", deadline: "Test"}];

    let loadingPromise = async () => {
        const response = await fetch(endpoint);
        return await response.json();
    }

    // onMount(async function () {
    //     const response = await fetch(endpoint);
    //     // rows = await response.json();
    // });
</script>
{#await loadingPromise()}
    <Spinner/>
{:then rows}
    <Table>
        <TableHead>
            <TableHeadCell>Tittel</TableHeadCell>
            <TableHeadCell>Sammendrag</TableHeadCell>
            <TableHeadCell>Rolle</TableHeadCell>
            <TableHeadCell>Kunde</TableHeadCell>
            <TableHeadCell>Frist</TableHeadCell>
            <TableHeadCell>Eksporter til Salesforce</TableHeadCell>
        </TableHead>
        <TableBody>
            {#each rows as row}
                <SummaryRow {row}/>
            {/each}
        </TableBody>
    </Table>
{:catch e}
    Klarte ikke å laste data
{/await}
