export interface StoryPrioritySuggestion {
  id: number;
  subject: string;
  ref: number;
  sprintId: number | null;
  sprintSlug: string | null;
  sprintName: string | null;
  sprintOrder: number | null;
}
