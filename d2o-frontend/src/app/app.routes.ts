import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: '/task' },
  { path: 'task', loadChildren: () => import('./components/task/task.routes').then(m => m.TASK_ROUTES) }
];
