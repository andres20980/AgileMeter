import { Injectable } from '@angular/core';
import { MatPaginatorIntl } from '@angular/material';
import { TranslateService } from '@ngx-translate/core';

@Injectable()
export class MatPaginationIntlService extends MatPaginatorIntl {
  translate: TranslateService;
  firstPageLabel = 'First page';
  itemsPerPageLabel = 'Items per page';
  lastPageLabel = 'Last page';
  nextPageLabel = 'Next page';
  previousPageLabel = 'Previous page';

  getRangeLabel = (page: number, pageSize: number, length: number): string => {
    const of = this.translate ? this.translate.instant('APP.OF') : 'of';
    if (length === 0 || pageSize === 0) {
      return '0 ' + of + ' ' + length;
    }
    length = Math.max(length, 0);
    const startIndex = ((page * pageSize) > length) ?
      (Math.ceil(length / pageSize) - 1) * pageSize:
      page * pageSize;

    const endIndex = Math.min(startIndex + pageSize, length);
    return startIndex + 1 + ' - ' + endIndex + ' ' + of + ' ' + length;
  };

  injectTranslateService(translate: TranslateService) {
    this.translate = translate;

    this.translate.onLangChange.subscribe(() => {
      this.translateLabels();
    });

    this.translateLabels();
  }

  translateLabels() {
    this.firstPageLabel = this.translate.instant('APP.FIRST_PAGE_LABEL');
    this.itemsPerPageLabel = this.translate.instant('APP.ITEMS_PER_PAGE_LABEL');
    this.lastPageLabel = this.translate.instant('APP.LAST_PAGE_LABEL');
    this.nextPageLabel = this.translate.instant('APP.NEXT_PAGE_LABEL');
    this.previousPageLabel = this.translate.instant('APP.PREVIOUS_PAGE_LABEL');
  }
}