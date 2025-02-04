<script lang="ts">
    import {Dropdown, DropdownItem, Search, Spinner} from 'flowbite-svelte';

    let {name, startingValue}: {name: string, startingValue: string} = $props();

    let w = $state(0);
    let dropdownOpen = $state(false);

    let searchTerm = $state(startingValue);
    let formValue: string | undefined = $state(undefined);

    let items: Promise<{name: string, id: string}[]> | null = $state(null)

    let timer: number;
    $effect(() => {
        clearTimeout(timer);
        if (searchTerm.length >= 3) {
            timer = setTimeout(() => {
                items = (async () => {
                    let response = await fetch(`http://localhost:5401/form-search?q=${searchTerm}`);
                    let result = await response.json();
                    return result;
                })()
            }, 500);
        } else {
            items = null;
        }
    });

    const handleChoice = (val: string, id: string) => {
        searchTerm = val;
        formValue = id;
        dropdownOpen = false;
    }
</script>

    <div bind:clientWidth={w}>
        <input hidden name={name} value={formValue}/>
    <Search size="md" bind:value={searchTerm} />
    <Dropdown placement="bottom-start" bind:open={dropdownOpen} class="overflow-y-auto max-h-48" trigger="click">
        <div style="width:{w}px">
            {#if items != null}
                {#await items}
                    <div class="flex justify-center">
                        <Spinner />
                    </div>
                {:then values}
                    {#if values.length === 0}
                        Ingen treff
                    {/if}
                    {#each values as value (value.name)}
                        <div>
                            <DropdownItem onclick={() => handleChoice(value.name, value.id)}>{value.name}</DropdownItem>
                        </div>
                    {/each}
                {/await}
            {/if}
            {#if items == null}
                <span class="py-8 px-2">Skriv minst 3 bokstaver for å starte søk</span>
            {/if}
        </div>
    </Dropdown>
</div>
