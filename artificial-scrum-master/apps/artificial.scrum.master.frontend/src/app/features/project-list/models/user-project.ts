export interface GetUserProjectsResponseElement {
  id: number;
  name: string;
  modifiedDate: Date;
  isPrivate: boolean;
  amOwner: boolean;
  ownerUsername: string;
}
