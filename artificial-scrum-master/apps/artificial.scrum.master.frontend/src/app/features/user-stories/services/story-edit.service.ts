import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { UserStoryDetails } from '../models/user-story-details';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StoryEditService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/userStories';

  public patchStoryDescription(
    storyId: number,
    version: number,
    description: string
  ): Observable<UserStoryDetails> {
    return this.httpClient.patch<UserStoryDetails>(
      `${this.baseApiUrl}/${storyId}`,
      {
        version,
        description,
      }
    );
  }
}
