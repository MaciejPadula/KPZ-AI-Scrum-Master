import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { GenerateTaskSuggestionsResponse } from '../models/get-task-suggestions-response';

@Injectable({
  providedIn: 'root'
})
export class TaskSuggestionsService {

  constructor() { }

  #httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/user-story/generate-tasks';

  getTaskSuggestions(title : string, description : string) {
    return this.#httpClient.post<GenerateTaskSuggestionsResponse>(`${this.baseApiUrl}`, {
      UserStoryTitle: title,
      UserStoryDescription: description,
    })
  }
}
