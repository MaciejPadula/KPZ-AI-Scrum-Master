import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { GetStoryPrioritizationSuggestionResponse } from '../models/get-story-srioritization-suggestion-response';
import { StoryPrioritySuggestion } from '../models/story-priority-suggestion';

@Injectable({
  providedIn: 'root',
})
export class StoriesPrioritySuggestionService {
  private readonly httpClient = inject(HttpClient);

  private readonly baseApiUrl = 'api/sprint/userStories/priority';

  public getSuggestedStoriesPriority(
    projectId: number,
    sprintId: number,
    generateAgain = false
  ): Observable<StoryPrioritySuggestion[]> {
    let url = `${this.baseApiUrl}?projectId=${projectId}&sprintId=${sprintId}`;
    if (generateAgain) {
      url += '&generateAgain=true';
    }
    return this.httpClient
      .get<GetStoryPrioritizationSuggestionResponse>(url)
      .pipe(map((response) => response.stories));
  }
}
