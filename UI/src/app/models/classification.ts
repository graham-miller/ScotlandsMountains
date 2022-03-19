export interface Classification {
    id: string;
    name: string;
    singularName: string;
    description: string;
    mountainsCount: number;
    mountains: MountainSummary[];
}

export interface MountainSummary {
    id: string;
    name: string;
    height: number;
    Longitude: number;
    Latitude: number;
}
