import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { UserStory } from '../models/user-story';
import { GetUserStoriesResponse } from '../models/get-user-stories-response';

@Injectable({
  providedIn: 'root',
})
export class UserStoriesService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/projects/userStories';

  public getUserStories(
    projectId: number,
    sprintId: number
  ): Observable<UserStory[]> {
    const params = new HttpParams()
      .set('projectId', projectId.toString())
      .set('sprintId', sprintId.toString());

    return this.httpClient
      .get<GetUserStoriesResponse>(`${this.baseApiUrl}`, { params: params })
      .pipe(map((response) => response.userStories));
  }
}
