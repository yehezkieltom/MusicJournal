import { tracks } from '$lib/stores/tracks';
import type { PageLoad } from './$types';

export const load: PageLoad = async () => {
    await tracks.load();
    return {}
}