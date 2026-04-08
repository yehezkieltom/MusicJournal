<script lang="ts">
    import { tracks } from '$lib/stores/tracks';
    import type { Track, TrackStatus } from '$lib/api';
  import { on } from 'svelte/events';

    let { track }: { track: Track } = $props();

    const STATUS_COLORS: Record<TrackStatus, string> = {
        Interested: '#3b82f6',
        Listening:  '#f59e0b',
        Pass:       '#6b7280',
        Loved:      '#10b981',
    };

    let editing = $state(false);
    let editTitle = $state(track.title);
    let editArtist = $state(track.artist);
    let editNotes = $state(track.notes ?? '');
    let saving = $state(false);

    async function saveEdit() {
        saving = true;
        await tracks.update(track.id, {
            ...track,
            title: editTitle,
            artist: editArtist,
            notes: editNotes,
        })

        editing = false;
        saving = false;
    }

    function cancelEdit() {
        editTitle = track.title;
        editArtist = track.artist;
        editNotes = track.notes ?? '';
        editing = false
    }

</script>

<div class="card">
    {#if editing}
    <!--Edit mode-->
    <div class="edit-form">
        <input bind:value={editTitle} placeholder="Title" />
        <input bind:value={editArtist} placeholder="Artist" />
        <textarea bind:value={editNotes} placeholder="Notes..."></textarea>
        <div class="actions">
            <button onclick={saveEdit} disabled={saving}>
                { saving ? 'Saving...' : 'Save' }
            </button>
            <button onclick={cancelEdit}>Cancel</button>
        </div>
    </div>
    {:else}
    <!--View mode-->
    <div class="track-info">
        <span
            class="status-dot"
            style="background: {STATUS_COLORS[track.status]}"
        ></span>
        <div>
            <strong>{track.title}</strong>
            <span>{track.artist}</span>
            {#if track.notes}
                <p class="notes">{track.notes}</p>
            {/if}
        </div>
    </div>
    <div class="actions">
        <select
            value={track.status}
            onchange={e => tracks.updateStatus(track.id, e.currentTarget.value as TrackStatus)}
        >
            <option value="Interested">Interested</option>
            <option value="Listening">Listening</option>
            <option value="Pass">Pass</option>
            <option value="Loved">Loved</option>
        </select>
        <button onclick={() => editing = true}>Edit</button>
        <button onclick={() => tracks.remove(track.id)}>Delete</button>
    </div>
    {/if}
</div>

<style>
    .card {
        display: flex;
        align-items: center;
        justify-content: space-between;
        padding: 0.75rem 1rem;
        border: 1px solid #e5e7eb;
        border-radius: 8px;
        margin-bottom: 0.5rem;
    }

    .track-info {
        display: flex;
        align-items: flex-start;
        gap: 0.75rem;
    }

    .status-dot {
        width: 10px;
        height: 10px;
        border-radius: 50%;
        margin-top: 5px;
        flex-shrink: 0;
    }

    .notes {
        font-size: 0.875rem;
        color: #6b7280;
        margin: 0.25rem 0 0;
    }

    .actions {
        display: flex;
        align-items: center;
        gap: 0.5rem
    }

    .edit-form {
        display: flex;
        flex-direction: column;
        gap: 0.5rem;
        width: 100%;
    }

</style>