import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable()
export class SchemaDataService {
    private baseUrl: string = 'http://linusnyren.ddns.net:8080/schema'

  constructor(private httpClient: HttpClient) { }

  test(schemaId: string, vehicle: string, team: string): Observable<string> {
    const url = this.getUrl(schemaId, vehicle, team);
    return this.httpClient.get(url, {responseType: 'text'})
      .pipe(
        map(value => value)
      );
  }

  getUrl(schemaId: string, vehicle: string, team: string): string {
    return `${this.baseUrl}/${schemaId}/${vehicle}/${team}`;
  }
}