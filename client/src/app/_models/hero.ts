import { Photo } from './photo';

export interface Hero {
  id: number;
  heroName: string;
  photoUrl: string;
  email: string;
  introduction: string;
  firstName: string;
  lastName: string;
  lastActive: Date;
  photos: Photo[];
}
