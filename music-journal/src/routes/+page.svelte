<script lang="ts">
    import { slide } from 'svelte/transition';
    import { tracks, filteredTracks, activeFilter } from '$lib/stores/tracks';
    import type { TrackStatus } from '$lib/api';
  import TrackCard from '$lib/components/TrackCard.svelte';

    const STATUSES = ['All', 'Interested', 'Listening', 'Pass', 'Loved'] as const;

    let newTrack = $state({
        title: '',
        artist: '',
        status: 'Interested' as TrackStatus
    });

    let error = $state<string | null>(null);
    let saving = $state(false);

    async function addTrack() {
        if (!newTrack.title || !newTrack.artist) return;
        
        saving = true;
        error = null;

        try {
            await tracks.add(newTrack);
            newTrack = {title: '', artist: '', status: 'Interested'}
        } catch (e) {
            error = e instanceof Error ? e.message : 'Something went wrong';
        } finally {
            saving = false;
        }
    }
</script>
<main>
    <h1 class="playwrite-ie-brand">Music Journal</h1>
    <!--Add form-->
    {#if error}
        <p class="error">{error}</p>
    {/if}
    <div class="add-form">
        <div class="form-background">
            <input class="left" bind:value={newTrack.title} placeholder="Title" />
            <input bind:value={newTrack.artist} placeholder="Artist" />
            <button class="main-save" onclick={addTrack} disabled={saving}>
                {#if saving}
                <span class="material-symbols-outlined">&#xE8b5;</span>
                {:else}
                <span class="material-symbols-outlined">&#xE145;</span>
                {/if}
            </button>
        </div>
    </div>

    <!--Filters-->
    <div class="filters">
        {#each STATUSES as status}
            <button
                onclick={() => activeFilter.set(status)}
                class:active={$activeFilter === status} //was using aria-pressed
            >
                {status}
            </button>
        {/each}
    </div>

    <!--Track list-->
    {#if $filteredTracks.length === 0}
        <div class="empty">
            {#if $activeFilter === 'All'}
                <p>No tracks yet. Add something you want to explore.</p>
            {:else}
                <p>No tracks with status "{$activeFilter}".</p>
            {/if}
        </div>
    {:else}
        {#each $filteredTracks as track (track.id)}
        <div transition:slide={{duration: 200}}>
            <TrackCard {track} />
        </div>
        {/each}
    {/if}
</main>

<style>
    /* fonts & icons */
    @import url('https://fonts.googleapis.com/css2?family=Playwrite+IE:wght@100..400&display=swap');
    @import url('https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200&icon_names=add');
    @import url('https://fonts.googleapis.com/css2?family=Material+Symbols+Outlined:opsz,wght,FILL,GRAD@20..48,100..700,0..1,-50..200&icon_names=schedule');
    
    main {
        max-width: 640px;
        margin: 2rem auto;
        padding: 0 1rem;
    }

    .material-symbols-outlined {
        font-variation-settings:
        'FILL' 0,
        'wght' 200,
        'GRAD' 200,
        'opsz' 40
    }

    .playwrite-ie-brand {
        font-family: "Playwrite IE", cursive;
        font-optical-sizing: auto;
        font-weight: 400;
        font-style: normal;
    }

    .filters {
        display: flex;
        gap: 0.5rem;
        margin-bottom: 1rem;

    }

    .filters button {
        font-size: .8rem;
        padding: 0.3rem 1rem;
        background-color: transparent;
        border: 1px solid grey;
        border-radius: 1rem;
    }

    .filters button.active {
        font-weight: 500;
        background-color: #aaa;
        border: none;
        /* border-bottom: 2px solid currentColor; */
    }

    .form-background {
        display: flex;
        height: 2.1rem;
        padding: 5px;
        background-color: #222;
        border-radius: 2rem;
        /* border: #333 solid 1px; */
        margin-bottom: 1rem;
        gap: 2px;
    }

    .form-background input {
        flex: 1;
        background-color: transparent;
        border: 2px solid transparent;
        border-radius: 2rem;
        color: #eee;
        padding: 0 1rem;
        font-size: .8rem;
        transition: border-color .1s ease-out;
    }
    .form-background input:hover {
        outline: none;
        border: 2px solid #fff;
        transition: border-color .13s ease-in;
        /* background-color: #555; */

    }
    .form-background input:focus {
        color: #000;
        outline: none;
        background-color: #fff;
    }
    
    .main-save {
        color: #eee;
        background-color: #222;
        border: 2px solid transparent;
        border-radius: 2rem;
        padding: 0.2rem;
        transition: border-color .1s ease-out;
    }
    .main-save:hover {
        border: 2px solid #fff;
        transition: border-color .13s ease-in;
    }

    .main-save:active {
        background-color: #fff;
        color:#222;
        transition: background-color .1 ease-in-out
    }

    .error { color: ef4444; }

    .empty {
        color: #9ca3af;
        text-align: center;
        padding: 2rem 0;
    }

</style>
