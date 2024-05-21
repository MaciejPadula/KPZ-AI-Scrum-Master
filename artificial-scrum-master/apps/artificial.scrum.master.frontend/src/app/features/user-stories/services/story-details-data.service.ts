import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserStoryDetails } from '../models/user-story-details';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StoryDetailsDataService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = '/api/userStories';

  public getStoryDetails(storyId: number): Observable<UserStoryDetails> {
    return this.httpClient.get<UserStoryDetails>(
      `${this.baseApiUrl}/${storyId}`
    );
  }
}
