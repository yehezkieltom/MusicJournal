import { writable, derived, get } from 'svelte/store';
import { tracksApi, type Track, type CreateTrackDto, type TrackStatus} from '$lib/api';

// Core store
const trackList = writable<Track[]>([]);

// Derived store - filtered view, updated automatically when trackList changes
export const activeFilter = writable<TrackStatus | 'All'>('All');

export const filteredTracks = derived(
    [trackList, activeFilter],
    ([$tracks, $filter]) => {
        if ($filter === 'All') return $tracks;
        return $tracks.filter(t => t.status === $filter);
    }
);

// Actions
export const tracks = {
    subscribe: trackList.subscribe,

    async load() {
        const data = await tracksApi.getAll();
        trackList.set(data);
    },

    async add(dto: CreateTrackDto) {
        const created = await tracksApi.create(dto);
        trackList.update(list => [...list, created]);
    },

    async updateStatus(id: number, status: TrackStatus) {
        const current = get(trackList).find(t => t.id === id);
        if (!current) return;

        await tracksApi.update(id, { ...current, status });
        trackList.update(list => 
            list.map(t => t.id === id ? { ...t, status } : t)
        );
    },

    async update(id: number, dto: Partial<CreateTrackDto>) {
        //superfluous since the editedTrack is Track or is it?
        // const current = get(trackList).find(t => t.id === id);
        // if (!current) return;

        await tracksApi.update(id, dto);
        trackList.update(list => 
            list.map(t => t.id === id ? {...t, ...dto} : t)
        );
    },

    async remove (id: number) {
        await tracksApi.delete(id);
        trackList.update(list => list.filter(t => t.id !== id));
    }
}