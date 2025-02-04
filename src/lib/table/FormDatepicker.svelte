<script lang="ts">
    import {Datepicker} from "flowbite-svelte";

    let {name, initialValue}: { name: string, initialValue?: string } = $props();

    let value = $state(initialValue ? new Date(initialValue) : undefined);
    let formValue = $derived.by(() => {
        if (!value) return value;
        const offset = value.getTimezoneOffset();
        const localDate = new Date(value.getTime() - offset * 60 * 1000);
        return localDate.toISOString().split('T')[0];
    })
</script>

<input hidden name={name} value={formValue}/>
<Datepicker bind:value locale="no-nb"/>