import { Directive, EventEmitter, Input, Output } from "@angular/core";

export type SortDirection = 'asc' | 'desc' | '';

export interface SortEvent {
    column: string;
    direction: SortDirection;
}

@Directive({
    selector: 'th[sortable]',
    host: {
        '[class.asc]': 'direction === "asc"',
        '[class.desc]': 'direction === "desc"',
        '(click)': 'rotate()'
    }
})
export class SortableHeader {

    nextRotation: { [key: string]: SortDirection } = { 'asc': 'desc', 'desc': '', '': 'asc' };

    @Input() sortable: string = '';
    @Input() direction: SortDirection = '';
    @Output() sort = new EventEmitter<SortEvent>();

    rotate() {
        this.direction = this.nextRotation[this.direction];
        this.sort.emit({ column: this.sortable, direction: this.direction });
    }
}