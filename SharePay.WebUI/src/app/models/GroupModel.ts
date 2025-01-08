import { UserViewModel } from "./UserViewModel";

export class GroupModel {
    id!: string;
    name!: string;
    owner!: UserViewModel | undefined;
    users!: UserViewModel[];
}