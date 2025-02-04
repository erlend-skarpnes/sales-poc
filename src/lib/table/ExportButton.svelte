<script lang="ts">
    import {Button, Modal} from "flowbite-svelte";
    import ExportForm from "./ExportForm.svelte";
    import type {DataRow} from "./dataRow";
    let modalOpen = $state(false);

    let {row}: {row: DataRow} = $props();

</script>

<Button on:click={() => modalOpen = true}>Eksporter</Button>

<Modal title="Eksporter til Salesforce" bind:open={modalOpen} autoclose={false}>
    <ExportForm {row}/>
</Modal>