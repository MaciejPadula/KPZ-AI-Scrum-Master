import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { GetSprintsResponse } from '../models/get-sprint-response';
import { Sprint } from '../models/sprint';

@Injectable({
  providedIn: 'root',
})
export class SprintDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/sprints';

  public getSprints(projectId: number): Observable<Sprint[]> {
    return this.httpClient
      .get<GetSprintsResponse>(`${this.baseApiUrl}/${projectId}`)
      .pipe(map((response) => response.sprints));
  }
}
