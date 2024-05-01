import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetSprintStats } from '../models/GetSprintStats';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SprintStatsService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/sprints';

  public getSprintStats(
    projectId: number,
    sprintId: number
  ): Observable<GetSprintStats> {
    return this.httpClient.get<GetSprintStats>(
      `${this.baseApiUrl}/${projectId}/stats?sprintId=${sprintId}`
    );
  }
}
