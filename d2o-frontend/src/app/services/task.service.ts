import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, tap, throwError } from 'rxjs';
import { Task } from '../models/task.model';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private apiUrl = 'https://localhost:7104/api/Tasks';
  private tasksSubject = new BehaviorSubject<Task[]>([]);
  tasks$ = this.tasksSubject.asObservable();

  constructor(private http: HttpClient) {
    this.loadTasks();
  }

  private loadTasks() {
    this.http.get<Task[]>(this.apiUrl).subscribe(
      (tasks) => this.tasksSubject.next(tasks),
      (error) => console.error('Error loading tasks', error)
    );
  }

  addTask(taskName: string): Observable<Task> {
    const newTask = { name: taskName, isCompleted: false };

    return this.http.post<Task>(this.apiUrl, newTask).pipe(
      tap((createdTask) => {
        const currentTasks = this.tasksSubject.value;
        this.tasksSubject.next([createdTask, ...currentTasks]);
      }),
      catchError((error) => {
        console.error('Error adding task:', error);
        return throwError(() => new Error('Failed to add task. Please try again later.'));
      })
    );
  }

  updateTask(id: string, name: string, isCompleted: boolean) {
    const updatedTask = { name, isCompleted };

    this.http.put(`${this.apiUrl}/${id}`, updatedTask).subscribe(
      () => {
        const updatedTasks = this.tasksSubject.value.map((task) =>
          task.id === id ? { ...task, name, isCompleted } : task
        );
        this.tasksSubject.next(updatedTasks);
      },
      (error) => console.error('Error updating task', error)
    );
  }

  deleteTask(id: string) {
    this.http.delete(`${this.apiUrl}/${id}`).subscribe(
      () => {
        const updatedTasks = this.tasksSubject.value.filter((task) => task.id !== id);
        this.tasksSubject.next(updatedTasks);
      },
      (error) => console.error('Error deleting task', error)
    );
  }
}
