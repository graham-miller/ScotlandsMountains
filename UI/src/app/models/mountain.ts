import { ClassificationSummary } from "./classification"
import { CountySummary } from "./county";
import { MapSummary } from "./map";
import { SectionSummary } from "./section";

export interface MountainSummary {
    id: string;
    name: string;
    height: number;
    longitude: number;
    latitude: number;
}

export interface Mountain {
    id: string;
    name: string;
    aliases: string[];
    gridRef: string;
    height: number;
    longitude: number;
    latitude: number;
    prominence: number;
    prominenceFrom: string;
    prominenceFromHeight: number;
    features: string;
    observations: string;
    parent: MountainSummary;
    section: SectionSummary;
    counties: CountySummary[];
    classifications: ClassificationSummary[];
    maps: MapSummary[];
}
