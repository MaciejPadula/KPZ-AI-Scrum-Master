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
import { MAT_DIALOG_DATA, MatDialog } from '@angular/material/dialog';
import { TranslateModule } from '@ngx-translate/core';
import { EditStoryDetailsComponent } from '../edit-story-details/edit-story-details.component';
import { StorySuggestionService } from '../../services/story-suggestion.service';
import { finalize } from 'rxjs';
import { EditorStateService } from '../../../../shared/services/editor-state.service';
import { StoryDetailsDataService } from '../../services/story-details-data.service';
import { ScrollService } from '../../../../shared/services/scroll.service';
import { GenerateTaskSuggestionsResponse } from '../../models/get-task-suggestions-response';
import { ToastService } from '../../../../shared/services/toast.service';
import { HttpClient } from '@angular/common/http';
import { TaskSuggestionsComponent } from '../task-suggestions/task-suggestions.component';
import { TaskSuggestionsService } from '../../services/task-suggestions.service';

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
    TaskSuggestionsComponent,
  ],
})
export class UserStoryDetailsComponent implements OnInit {
  @ViewChild('userStoryDescription', { read: ElementRef })
  storyDescription: ElementRef;
  @ViewChild('userStoryEditor', { read: ElementRef })
  storyEditor: ElementRef;

  private readonly storyDetailsDataService = inject(StoryDetailsDataService);
  private readonly storySuggestionService = inject(StorySuggestionService);
  private readonly editorStateServiceService = inject(EditorStateService);
  private readonly scrollService = inject(ScrollService);
  private readonly taskSuggestionsService = inject(TaskSuggestionsService);

  details = signal<UserStoryDetails | null>(null);
  taskSuggestions = signal<GenerateTaskSuggestionsResponse | null>(null);
  error = signal<boolean>(false);

  #isLoading = signal<boolean>(false);
  public isLoading = this.#isLoading.asReadonly();

  storyId: number;
  projectId: number;
  suggestionsOpen = signal(false);
  private readonly toastService = inject(ToastService);

  private readonly dialog = inject(MatDialog);

  constructor(
    @Inject(MAT_DIALOG_DATA) data: { userStoryId: number; projectId: number }
  ) {
    this.storyId = data.userStoryId;
    this.projectId = data.projectId;
  }

  private readonly httpClient = inject(HttpClient);
  private readonly baseApiUrl = 'api/user-story/generate-tasks';

  ngOnInit(): void {
    this.storyDetailsDataService.getStoryDetails(this.storyId).subscribe({
      next: (response) => {
        this.details.set(response);
        this.editorStateServiceService.setDescriptionEditorValue(
          response.description ?? ''
        );
      },
      error: () => this.error.set(true),
    });
  }

  generateSuggestion() {
    if (this.details() == null || !this.details()?.title) {
      return;
    }
    this.#isLoading.set(true);
    this.scrollService.scrollToElement({
      element: this.storyEditor,
      timeout: 100,
    });
    this.storySuggestionService
      .getStorykDescriptionSuggestion(
        this.details()?.title ?? '',
        this.details()?.description ?? null
      )
      .pipe(
        finalize(() => {
          this.#isLoading.set(false);
          this.editorStateServiceService.setSuggestionsVisible(true);
          this.scrollService.scrollToElement({
            element: this.storyEditor,
            timeout: 100,
          });
        })
      )
      .subscribe({
        next: (response) =>
          this.editorStateServiceService.setSuggestionString(
            response.descriptionEditSuggestion
          ),
        error: () => this.error.set(true),
      });
  }

  toggleDescriptionEditor() {
    if (this.editorStateServiceService.isEditorVisible()) {
      this.editorStateServiceService.setEditorVisible(false);
      this.scrollService.scrollToElement({
        element: this.storyDescription,
        block: 'start',
      });
      return;
    }
    this.editorStateServiceService.setEditorVisible(true);
    this.scrollService.scrollToElement({
      element: this.storyEditor,
      timeout: 100,
    });
  }

  updateStoryDetails(detailsUpdate: UserStoryDetails) {
    this.details.set(detailsUpdate);
  }
  generateTaskSuggestions() {
    if (this.details() == null || !this.details()?.description) {
      return;
    }
    this.suggestionsOpen.set(true);

    this.taskSuggestionsService
      .getTaskSuggestions(this.details()!.title, this.details()!.description)
      .subscribe({
        next: (response) => this.taskSuggestions.set(response),
        error: () => this.error.set(true),
      });
  }

  closeSuggestions() {
    this.suggestionsOpen.set(false);
    this.taskSuggestions.set(null);
  }
}
