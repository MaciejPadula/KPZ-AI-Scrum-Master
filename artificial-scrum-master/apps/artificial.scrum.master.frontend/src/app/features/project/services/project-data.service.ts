import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { ProfileTimelineEvent } from '../models/profile-timeline-event';
import { GetProfileTimeLineResponse } from '../models/get-profile-timeline-response';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class ProjectDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/timeline';

  public getProjectEvents(projectId: number): Observable<ProfileTimelineEvent[]> {
    return this.httpClient
      .get<GetProfileTimeLineResponse>(`${this.baseApiUrl}/${projectId}`)
      .pipe(map((response) => response.timeLineEvents));
  }
}
