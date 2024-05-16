import { Injectable, signal } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class EditorStateService {
  #suggestionString = signal<string | null>(null);
  suggestionString = this.#suggestionString.asReadonly();

  #descriptionEditorValue = signal<string>('');
  descriptionEditorValue = this.#descriptionEditorValue.asReadonly();

  #isSuggestionsVisible = signal(false);
  public isSuggestionsVisible = this.#isSuggestionsVisible.asReadonly();

  #isEditorVisible = signal(false);
  public isEditorVisible = this.#isEditorVisible.asReadonly();

  setDescriptionEditorValue(value: string) {
    this.#descriptionEditorValue.set(value);
  }

  setSuggestionString(value: string | null) {
    this.#suggestionString.set(value);
  }

  setSuggestionsVisible(value: boolean) {
    this.#isSuggestionsVisible.set(value);
  }

  setEditorVisible(value: boolean) {
    this.#isEditorVisible.set(value);
  }

  updateDescription(newValue: string) {
    this.#descriptionEditorValue.set(newValue);
  }

  rejectSuggestion() {
    this.#isSuggestionsVisible.set(false);
  }

  replaceWithSuggestion() {
    this.#descriptionEditorValue.set(this.suggestionString() ?? '');
    this.#isSuggestionsVisible.set(false);
    if (!this.#isEditorVisible()) {
      this.#isEditorVisible.set(true);
    }
  }

  appendSuggestionToBack() {
    this.#descriptionEditorValue.set(
      this.descriptionEditorValue().concat('\n', this.suggestionString() ?? '')
    );
    this.#isSuggestionsVisible.set(false);
    if (!this.#isEditorVisible()) {
      this.#isEditorVisible.set(true);
    }
  }

  appendSuggestionToFront() {
    this.#descriptionEditorValue.set(
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
    this.#descriptionEditorValue.set(originalValue);
  }

  resetEditorState() {
    this.#isSuggestionsVisible.set(false);
    this.#isEditorVisible.set(false);
    this.#suggestionString.set(null);
    this.#descriptionEditorValue.set('');
  }
}
