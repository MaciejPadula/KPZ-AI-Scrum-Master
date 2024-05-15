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
import { StorySuggestionService } from '../../services/story-suggestion.service';
import { finalize } from 'rxjs';
import { EditorStateServiceService } from '../../../../shared/services/editor-state-service.service';

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
  private readonly editorStateServiceService = inject(
    EditorStateServiceService
  );

  details = signal<UserStoryDetails | null>(null);
  error = signal<boolean>(false);

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
          this.editorStateServiceService.setDescriptionEditorValue =
            response.description ?? '';
        },
        error: () => this.error.set(true),
      });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.title) {
      return;
    }
    this.#isLoading.set(true);
    setTimeout(() => this.scrollToElement(this.storyEditor), 50);

    this.storySuggestionService
      .getStorykDescriptionSuggestion(
        this.details()?.title ?? '',
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.editorStateServiceService.suggestionsVisible = true;
          setTimeout(() => this.scrollToElement(this.storyEditor), 50);
        })
      )
      .subscribe({
        next: (response) =>
          (this.editorStateServiceService.setSuggestionString =
            response.descriptionEditSuggestion),
        error: () => this.error.set(true),
      });
  }

  toggleDescriptionEditor() {
    if (this.editorStateServiceService.isEditorVisible()) {
      this.editorStateServiceService.editorVisible = false;
      this.scrollToElement(this.storyDescription, 'start');
      return;
    }
    this.editorStateServiceService.editorVisible = true;
    setTimeout(() => this.scrollToElement(this.storyEditor), 50);
  }

  updateStoryDetails($event: UserStoryDetails) {
    this.details.set($event);
  }

  private scrollToElement(element: ElementRef, block: 'end' | 'start' = 'end') {
    if (element && element.nativeElement) {
      element.nativeElement.scrollIntoView({
        behavior: 'smooth',
        block: block,
        inline: 'nearest',
      });
    }
  }
}
