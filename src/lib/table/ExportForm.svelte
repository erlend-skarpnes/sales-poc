<script lang="ts">
    import type {DataRow} from "./dataRow";
    import {Button, Helper, Input, Label, Select, Spinner, Textarea} from "flowbite-svelte";
    import type {EventHandler} from "svelte/elements";
    import SearchInput from "./SearchInput.svelte";
    import FormDatepicker from "./FormDatepicker.svelte";

    let {row}: { row: DataRow } = $props();

    type SaveState = "unsaved" | "saving" | "saved"
    let saveState: SaveState = $state("unsaved")

    type Error = { name: string, message: string }
    let errors: Error[] = $state([])

    let buttonDisabled = $derived.by(() => {
        switch (saveState) {
            case "unsaved":
                return false;
            case "saving":
            case "saved":
                return true;
        }
    })

    let buttonText = $derived.by(() => {
        switch (saveState) {
            case "unsaved":
                return "Lagre";
            case "saved":
                return "Lagret ✅";
            case "saving":
                return "Lagrer...";
        }
    });

    const endpoint = "http://localhost:5401/form-input";
    const pickListData = (async () => {
        const response = await fetch(endpoint);
        return await response.json();
    })();

    const hasError = (name: string) => {
        return errors.some(error => error.name.toLowerCase() == name)
    }

    const getErrorMessage = (name: string) => {
        return errors.find(error => error.name.toLowerCase() == name)?.message;
    }

    const onSubmit: EventHandler<SubmitEvent, HTMLFormElement> = async (e) => {
        e.preventDefault();
        if (e?.currentTarget == null) return;
        const formData = new FormData(e.currentTarget);

        saveState = "saving";

        var response = await fetch("http://localhost:5401/form", {
            method: "POST",
            body: formData
        });

        if (response.ok) {
            saveState = "saved";
        } else {
            saveState = "unsaved";
            errors = await response.json();
        }
    }

</script>

<form onsubmit={onSubmit} class="w-full flex flex-col gap-4">
    {#key errors}
        {#await pickListData}
            <Spinner/>
        {:then values}
            <div>
                <Label for="name">Navn</Label>
                <Input name="name" value={row.title ?? ""}></Input>
                {#if hasError("name")}
                    <Helper class="mt-2" color="red">
                        {getErrorMessage("name")}
                    </Helper>
                {/if}
            </div>
            <div>
                <Label for="description">Beskrivelse</Label>
                <Textarea name="description" value={row.summary ?? ""} class="h-32"></Textarea>
            </div>
            <div>
                <Label for="account">Kunde</Label>
                <SearchInput name="account" startingValue={row.customer}/>
            </div>

            <div>
                <Label for="opportunityType">Salgsmulighetstype</Label>
                <Select name="opportunityType"
                        items={values["picklistFieldValues"]["Opportunity_Type__c"].values.map((val) => ({value: val["value"], name: val["label"]}))}></Select>
            </div>
            <div>
                <Label for="milesOffice">Mileskontor</Label>
                <Select name="milesOffice"
                        items={values["picklistFieldValues"]["Location__c"].values.map((val) => ({value: val["value"], name: val["label"]}))}></Select>
            </div>
            <div>
                <Label for="stage">Stage</Label>
                <Select name="stage"
                        items={values["picklistFieldValues"]["StageName"].values.map((val) => ({value: val["value"], name: val["label"]}))}></Select>
            </div>
            <div>
                <Label for="contractValue">Kontraktsverdi</Label>
                <Input name="contractValue" type="number"/>
            </div>
            <div>
                <Label for="closeDate">Frist</Label>
                <FormDatepicker name="closeDate" initialValue={row.deadline}/>
            </div>
            <div>
                <Label for="startDate">Start</Label>
                <FormDatepicker name="startDate"/>
            </div>
            <div>
                <Label for="endDate">Slutt</Label>
                <FormDatepicker name="endDate"/>
            </div>
        {/await}
        <div class="flex justify-end">
            <Button type="submit">{buttonText}</Button>
        </div>
    {/key}
</form>
