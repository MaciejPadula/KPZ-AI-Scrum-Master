import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { map, Observable } from 'rxjs';
import { GetProfileTimeLineResponse } from '../models/get-profile-timeline-response';
import { ProfileTimelineEvent } from '../models/profile-timeline-event';

@Injectable({
  providedIn: 'root',
})
export class FeedDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/timeline';

  public getFeedEvents(): Observable<ProfileTimelineEvent[]> {
    return this.httpClient
      .get<GetProfileTimeLineResponse>(`${this.baseApiUrl}`)
      .pipe(map((response) => response.timeLineEvents));
  }
}
