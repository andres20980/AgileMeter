import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS, HttpBackend } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

@Injectable({providedIn: 'root'})
export class HttpClientTrans extends HttpClient {
  constructor(handler: HttpBackend) {
    super(handler);
  }
}

export function HttpLoaderFactory(httpClient: HttpClientTrans) {
  return new TranslateHttpLoader(httpClient, "/i18n/", ".json");
}