const BASE_URL = 'http://localhost:5074/api';

export type TrackStatus = 'Interested' | 'Listening' | 'Pass' | 'Loved';

export interface Track {
    id: number;
    title: string;
    artist: string;
    album?: string;
    genre?: string;
    sourceUrl?: string;
    notes?: string;
    releaseYear?: number;
    status: TrackStatus;
    dateAdded: string;
    lastUpdated?: string;
}

export interface CreateTrackDto {
    title: string;
    artist: string;
    album?: string;
    genre?: string;
    sourceUrl?: string;
    notes?: string;
    releaseYear?: number;
    status: TrackStatus;
}

async function request<T>(path: string, options?: RequestInit): Promise<T> {
    const res =  await fetch(`${BASE_URL}${path}`, {
        headers: { 'Content-Type': 'application/json' },
        ...options,
    });

    if (!res.ok) {
        const message = await res.text();
        throw new Error(`API error ${res.status}: ${message}`);
    }

    //204 No Content (DELETE) has no body
    if (res.status === 204) return undefined as T;

    return res.json()
}

export const tracksApi = {
    getAll: () =>
        request<Track[]>('/tracks'),

    getById: (id: number) =>
        request<Track>(`/tracks/${id}`),

    create: (data: CreateTrackDto) => 
        request<Track>('/tracks', {
            method: 'POST',
            body: JSON.stringify(data),
        }),

    update: (id: number, data: Partial<CreateTrackDto>) =>
        request<void>(`/tracks/${id}`, {
            method: 'PUT',
            body: JSON.stringify(data),
        }),

    delete: (id: number) =>
        request<void>(`/tracks/${id}`, { method: 'DELETE' }),
}