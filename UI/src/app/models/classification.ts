import { MountainSummary } from "./mountain";

export interface ClassificationSummary {
    id: string;
    name: string;
    description: string;
    mountainsCount: number;
}

export interface Classification {
    id: string;
    name: string;
    singularName: string;
    description: string;
    mountainsCount: number;
    mountains: MountainSummary[];
}
