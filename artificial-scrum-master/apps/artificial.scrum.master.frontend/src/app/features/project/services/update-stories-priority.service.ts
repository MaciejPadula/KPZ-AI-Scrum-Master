import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { UpdateSprintOrderRequest } from '../models/update-sprint-order-request';
import { StoryPrioritySuggestion } from '../models/story-priority-suggestion';

@Injectable({
  providedIn: 'root',
})
export class UpdateStoriesPriorityService {
  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/projects/sprints/priority';

  public updateStoriesPriority(
    projectId: number,
    sprintId: number,
    storyPrioritySuggestions: StoryPrioritySuggestion[]
  ): Observable<void> {
    const storyIds = storyPrioritySuggestions.map((s) => s.id);
    const request: UpdateSprintOrderRequest = {
      storyIds,
      sprintId,
      projectId,
    };
    return this.httpClient.post<void>(this.baseApiUrl, request);
  }
}
