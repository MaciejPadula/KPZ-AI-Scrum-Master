export interface GenerateTaskSuggestionsResponse {
    tasks: TaskSuggestion[];
}

export interface TaskSuggestion {
    title: string;
    description: string;
    accepted: boolean | null;
}