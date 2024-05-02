export interface TaskDetails {
  taskId: number;
  subject: string;
  taskRef: number;
  tags: string[][];
  descriptionHtml: string | null;
  createdDate: Date;
  finishedDate: Date | null;
  milestoneSlug: string;
  statusName: string;
  statusColor: string;
  ownerUsername: string;
  ownerFullName: string;
  ownerPhoto: string | null;
  assignedToUsername: string | null;
  assignedToFullName: string | null;
  assignedToPhoto: string | null;
  userStoryRef: number;
  userStorySubject: string;
}
