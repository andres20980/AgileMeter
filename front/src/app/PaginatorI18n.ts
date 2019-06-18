import { MatPaginatorIntl } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

export class PaginatorI18n {

    constructor(private readonly translate: TranslateService) {}

    getPaginatorIntl(): MatPaginatorIntl {
        const paginatorIntl = new MatPaginatorIntl();

        console.log(this.translate.currentLang);
        console.log(this.translate);

        paginatorIntl.itemsPerPageLabel = this.translate.instant('APP.ITEMS_PER_PAGE_LABEL');
        paginatorIntl.nextPageLabel = this.translate.instant('APP.NEXT_PAGE_LABEL');
        paginatorIntl.previousPageLabel = this.translate.instant('APP.PREVIOUS_PAGE_LABEL');
        paginatorIntl.firstPageLabel = this.translate.instant('APP.FIRST_PAGE_LABEL');
        paginatorIntl.lastPageLabel = this.translate.instant('APP.LAST_PAGE_LABEL');
        paginatorIntl.getRangeLabel = this.getRangeLabel.bind(this);
        return paginatorIntl;
    }

    private getRangeLabel(page: number, pageSize: number, length: number): string {
        if (length === 0 || pageSize === 0) {
            return this.translate.instant('APP.RANGE_PAGE_LABEL_1', { length });
        }
        length = Math.max(length, 0);
        const startIndex = page * pageSize;
        // If the start index exceeds the list length, do not try and fix the end index to the end.
        const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
        return this.translate.instant('APP.RANGE_PAGE_LABEL_2', { startIndex: startIndex + 1, endIndex, length });
    }
}