export interface StoryPrioritySuggestion {
  userStoryId: number | null;
  userStorySubject: string | null;
  userStoryRef: number | null;
  sprintId: number;
  sprintSlug: string | null;
  sprintName: string | null;
  sprintOrder: number | null;
}
