import { Component, computed, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DiffEditorModel, MonacoEditorModule } from 'ngx-monaco-editor-v2';

@Component({
  selector: 'app-description-diff',
  standalone: true,
  imports: [CommonModule, MonacoEditorModule],
  templateUrl: './description-diff.component.html',
})
export class DescriptionDiffComponent {
  originalData = input.required<string | null | undefined>();
  modifiedData = input.required<string | null | undefined>();

  private readonly language = 'markdown';

  options = computed(() => {
    return {
      theme: document.body.classList.contains('dark-theme')
        ? 'vs-dark'
        : 'vs-light',
      readOnly: true,
      lineNumbers: 'off',
      wordWrap: 'on',
      wrappingIndent: 'indent',
    };
  });

  originalModel = computed(
    () =>
      ({
        code: this.originalData() ?? '',
        language: this.language,
      } as DiffEditorModel)
  );

  modifiedModel = computed(
    () =>
      ({
        code: this.modifiedData() ?? '',
        language: this.language,
      } as DiffEditorModel)
  );
}
