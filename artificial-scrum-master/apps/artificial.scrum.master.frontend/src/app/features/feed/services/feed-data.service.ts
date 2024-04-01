import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, of } from 'rxjs';
import { GetProfileTimeLineResponse } from '../models/get-profile-timeline-response';
import { ProfileTimelineEvent } from '../models/profile-timeline-event';
import { ScrumObjectType } from '../../../shared/models/scrum-object-type';
import { ScrumObjectState } from '../../../shared/models/scrum-object-state';

@Injectable({
  providedIn: 'root',
})
export class FeedDataService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/timeline';

  public getFeedEvents(): Observable<ProfileTimelineEvent[]> {
    return of(this.rows.timeLineEvents);
    // return this.httpClient
    //   .get<GetProfileTimeLineResponse>(`${this.baseApiUrl}`)
    //   .pipe(map((response) => response.timeLineEvents));
  }

  private rows: GetProfileTimeLineResponse = {
    timeLineEvents: [
      {
        userName: 'John Doe',
        userNick: 'johndoe',
        userAvatar: 'https://media-protected.taiga.io/user/5/c/a/b/04511999f07ed0df9ba4eaec436903679b5aa9cd86373171ac72f9eb05d3/a62072794c4957a78c31e134503ace0f.jpg.80x80_q85_crop.jpg?token=ZgseJg%3AZaVqQH7ovsi95uijWvNzFp8GCha5MbBk4Oq8OtspS93RvVKga-nbfPqsI-ipMCsqDjzzNSP4O_C5dsTtqUwpBA',
        scrumObjectType: ScrumObjectType.Task,
        scrumObjectId: 1,
        scrumObjectName: 'Task 1',
        scrumObjectState: ScrumObjectState.Changed,
        projectId: 1,
        projectName: 'Project 1',
      },
      {
        userName: 'John Doe',
        userNick: 'johndoe',
        userAvatar: 'https://media-protected.taiga.io/user/5/c/a/b/04511999f07ed0df9ba4eaec436903679b5aa9cd86373171ac72f9eb05d3/a62072794c4957a78c31e134503ace0f.jpg.80x80_q85_crop.jpg?token=ZgseJg%3AZaVqQH7ovsi95uijWvNzFp8GCha5MbBk4Oq8OtspS93RvVKga-nbfPqsI-ipMCsqDjzzNSP4O_C5dsTtqUwpBA',
        scrumObjectType: ScrumObjectType.Task,
        scrumObjectId: 1,
        scrumObjectName: 'Task 1',
        scrumObjectState: ScrumObjectState.Created,
        projectId: 1,
        projectName: 'Project 1',
      },
      {
        userName: 'John Doe',
        userNick: 'johndoe',
        userAvatar: 'https://media-protected.taiga.io/user/5/c/a/b/04511999f07ed0df9ba4eaec436903679b5aa9cd86373171ac72f9eb05d3/a62072794c4957a78c31e134503ace0f.jpg.80x80_q85_crop.jpg?token=ZgseJg%3AZaVqQH7ovsi95uijWvNzFp8GCha5MbBk4Oq8OtspS93RvVKga-nbfPqsI-ipMCsqDjzzNSP4O_C5dsTtqUwpBA',
        scrumObjectType: ScrumObjectType.UserStory,
        scrumObjectId: 1,
        scrumObjectName: 'Historyjka 1',
        scrumObjectState: ScrumObjectState.Changed,
        projectId: 1,
        projectName: 'Project 1',
      },
      {
        userName: 'John Doe',
        userNick: 'johndoe',
        userAvatar: 'https://media-protected.taiga.io/user/5/c/a/b/04511999f07ed0df9ba4eaec436903679b5aa9cd86373171ac72f9eb05d3/a62072794c4957a78c31e134503ace0f.jpg.80x80_q85_crop.jpg?token=ZgseJg%3AZaVqQH7ovsi95uijWvNzFp8GCha5MbBk4Oq8OtspS93RvVKga-nbfPqsI-ipMCsqDjzzNSP4O_C5dsTtqUwpBA',
        scrumObjectType: ScrumObjectType.UserStory,
        scrumObjectId: 1,
        scrumObjectName: 'Historyjka 1',
        scrumObjectState: ScrumObjectState.Created,
        projectId: 1,
        projectName: 'Project 1',
      }
    ]
  };
}
