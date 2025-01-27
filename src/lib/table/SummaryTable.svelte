<script lang="ts">
    import {
        Table,
        TableBody,
        TableBodyCell,
        TableBodyRow,
        TableHead,
        TableHeadCell,
        Checkbox,
        TableSearch,
        Button
    } from 'flowbite-svelte';
    import SummaryRow from "./SummaryRow.svelte";

    import { onMount } from "svelte";
    import type {DataRow} from "./dataRow";

    const endpoint = "http://localhost:5401/summaries";
    let rows: DataRow[] = [{title: "Test", customer: "Test", role: "Test", summary: "Test", deadline: "Test"}];

    onMount(async function () {
        const response = await fetch(endpoint);
        rows = await response.json();
    });
</script>

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
                <SummaryRow {row} />
            {/each}
        </TableBody>
    </Table>