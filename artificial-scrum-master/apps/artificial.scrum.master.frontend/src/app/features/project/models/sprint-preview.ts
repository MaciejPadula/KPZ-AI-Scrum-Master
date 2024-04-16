export interface SprintPreview {
  ProjectId: number;
  ProjectName: string;
  ProjectSlug: string;
  SprintId: number;
  SprintName: string;
  EstimatedStart: Date;
  EstimatedEnd: Date;
  UserStories: UserStoryPrweview[];
}

interface UserStoryPrweview {
  UserStoryId: number;
  UserStoryName: string;
  StatusName: string;
  IsClosed: boolean;
  UserStoryRef: number;
  UserStoryPoints: number;
}
