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
    import ExportButton from "./ExportButton.svelte";
    import type {DataRow} from "./dataRow";

    let { row }: {row: DataRow} = $props();
</script>
<TableBodyRow>
                <TableBodyCell tdClass="wrap">{row.title}</TableBodyCell>
                <TableBodyCell tdClass="wrap">{row.summary}</TableBodyCell>
                <TableBodyCell>{row.role}</TableBodyCell>
                <TableBodyCell>{row.customer}</TableBodyCell>
                <TableBodyCell>{row.deadline}</TableBodyCell>
                <TableBodyCell><ExportButton /></TableBodyCell>
            </TableBodyRow>