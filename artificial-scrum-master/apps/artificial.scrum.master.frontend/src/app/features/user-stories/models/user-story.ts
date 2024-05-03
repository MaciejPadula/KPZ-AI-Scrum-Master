export interface UserStory {
  userStoryId: number;
  userStorySubject: string;
  statusName: string;
  statusColor: string;
  userStoryRef: number;
  isClosed: boolean;
  assignedToUsername: string;
  assignedToFullNameDisplay: string;
  assignedToPhoto: string;
  ownerUsername: string;
  ownerFullNameDisplay: string;
  ownerPhoto: string;
  sprintId: number;
  sprintSlug: string;
  sprintName: string;
  totalPoints: number;
}
