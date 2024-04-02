import { ScrumObjectState } from "../../../shared/models/scrum-object-state";
import { ScrumObjectType } from "../../../shared/models/scrum-object-type";

export interface ProfileTimelineEvent {
  userName: string;
  userNick: string | null;
  userAvatar: string;

  scrumObjectType: ScrumObjectType;
  scrumObjectId: number;
  scrumObjectName: string;
  scrumObjectState: ScrumObjectState;

  projectId: number | null;
  projectName: string | null;
}