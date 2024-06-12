import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class StoryStateService {
  private readonly refreshSubject$ = new Subject<void>();
  public refresh$ = this.refreshSubject$.asObservable();

  public refresh(): void {
    this.refreshSubject$.next();
  }
}
