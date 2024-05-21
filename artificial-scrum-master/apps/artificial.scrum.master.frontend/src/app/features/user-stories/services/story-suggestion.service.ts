import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GetStorySuggestion } from '../models/get-story-suggestion';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StorySuggestionService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/userStory/suggestions';

  public getStorykDescriptionSuggestion(
    userStoryTitle: string | null,
    userStoryDescription: string | null
  ): Observable<GetStorySuggestion> {
    return this.httpClient.post<GetStorySuggestion>(`${this.baseApiUrl}`, {
      storyTitle: userStoryTitle,
      storyDescription: userStoryDescription,
    });
  }
}
