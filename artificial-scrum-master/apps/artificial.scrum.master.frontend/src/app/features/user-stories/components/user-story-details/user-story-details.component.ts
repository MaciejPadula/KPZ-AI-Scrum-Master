import {
  Component,
  ElementRef,
  Inject,
  inject,
  OnInit,
  signal,
  ViewChild,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { MaterialModule } from './../../../../shared/material.module';
import { UserStoryDetails } from '../../models/user-story-details';
import { HttpClient } from '@angular/common/http';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { EditStoryDetailsComponent } from '../edit-story-details/edit-story-details.component';
import { GetStorySuggestion } from '../../models/get-story-suggestion';
import { StorySuggestionService } from '../../services/story-suggestion.service';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-user-story-details',
  standalone: true,
  templateUrl: './user-story-details.component.html',
  styleUrls: ['./user-story-details.component.scss'],
  imports: [
    CommonModule,
    MaterialModule,
    TranslateModule,
    EditStoryDetailsComponent,
  ],
})
export class UserStoryDetailsComponent implements OnInit {
  @ViewChild('description', { read: ElementRef })
  storyDescription: ElementRef;
  @ViewChild('editor', { read: ElementRef })
  storyEditor: ElementRef;

  private readonly storySuggestionService = inject(StorySuggestionService);

  details = signal<UserStoryDetails | null>(null);
  error = signal<boolean>(false);

  isSuggestionsVisible = signal(false);
  suggestion = signal<GetStorySuggestion | null>(null);
  isEditorVisible = signal(false);
  descriptionEditorValue = signal<string>('');

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  #httpClient = inject(HttpClient);
  #storyId: number;
  public readonly storyId: number;

  constructor(@Inject(MAT_DIALOG_DATA) storyId: number) {
    this.#storyId = storyId;
    this.storyId = storyId;
  }

  ngOnInit(): void {
    this.#httpClient
      .get<UserStoryDetails>(`/api/userStories/${this.#storyId}`)
      .subscribe({
        next: (response) => {
          this.details.set(response);
          this.descriptionEditorValue.set(response.description ?? '');
        },
        error: () => this.error.set(true),
      });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.title) {
      return;
    }
    
    this.#isLoading.set(true);
    this.storySuggestionService
      .getStorykDescriptionSuggestion(
        this.details()?.title ?? '',
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.isSuggestionsVisible.set(true);
          setTimeout(() => this.scrollToElement(this.storyEditor), 50);
        })
      )
      .subscribe({
        next: (response) => this.suggestion.set(response),
        error: () => this.error.set(true),
      });
  }

  openDescriptionEditor() {
    if (this.isEditorVisible()) {
      setTimeout(() => this.isEditorVisible.set(false), 100);
      setTimeout(() => this.scrollToElement(this.storyDescription), 50);
      return;
    }
    setTimeout(() => this.isEditorVisible.set(true), 50);
    setTimeout(() => this.scrollToElement(this.storyEditor), 100);
  }

  private scrollToElement(element: ElementRef) {
    if (element && element.nativeElement) {
      element.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: 'start',
      });
    }
  }
}
