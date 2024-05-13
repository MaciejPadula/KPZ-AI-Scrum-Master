import { signal } from '@angular/core';

export class EditorStateServiceService {
  suggestionString = signal<string | null>(null);
  descriptionEditorValue = signal<string>('');

  #isSuggestionsVisible = signal(false);
  public isSuggestionsVisible = this.#isSuggestionsVisible.asReadonly();

  #isEditorVisible = signal(false);
  public isEditorVisible = this.#isEditorVisible.asReadonly();

  set suggestionsVisible(value: boolean) {
    this.#isSuggestionsVisible.set(value);
  }

  set editorVisible(value: boolean) {
    this.#isEditorVisible.set(value);
  }

  updateDescription(newValue: string) {
    this.descriptionEditorValue.set(newValue);
  }

  rejectSuggestion() {
    this.#isSuggestionsVisible.set(false);
  }

  replaceWithSuggestion() {
    this.descriptionEditorValue.set(this.suggestionString() ?? '');
    this.#isSuggestionsVisible.set(false);
    if (!this.#isEditorVisible()) {
      this.#isEditorVisible.set(true);
    }
  }

  appendSuggestionToBack() {
    this.descriptionEditorValue.set(
      this.descriptionEditorValue().concat('\n', this.suggestionString() ?? '')
    );
    this.#isSuggestionsVisible.set(false);
    if (!this.#isEditorVisible()) {
      this.#isEditorVisible.set(true);
    }
  }

  appendSuggestionToFront() {
    this.descriptionEditorValue.set(
      (this.suggestionString() ?? '').concat(
        '\n',
        this.descriptionEditorValue()
      )
    );
    this.#isSuggestionsVisible.set(false);
    if (!this.#isEditorVisible()) {
      this.#isEditorVisible.set(true);
    }
  }

  resetDescription(originalValue: string) {
    this.#isEditorVisible.set(false);
    this.descriptionEditorValue.set(originalValue);
  }
}
