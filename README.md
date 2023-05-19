# Interview Ace

Interview Ace is a web-based application built on the .NET MVC framework and powered by a MySQL database. Its primary objective is to assist job seekers, particularly new graduates and students, in finding suitable job opportunities and enhancing their interview skills. The application incorporates various essential features, including a recommendation system, a chatbot for interview questions, and a facial expression recognition system for feedback on emotional responses during interview practice.

## Purpose

The job search and interview process can be overwhelming for job seekers. Interview Ace aims to alleviate these challenges by providing a personalized and interactive platform that enables users to practice and improve their interview skills, ultimately increasing their chances of securing employment.

## Features

### Recommendation System

The recommendation system in Interview Ace employs the TF-IDF (Term Frequency-Inverse Document Frequency) vectorization technique. It analyzes job descriptions and matches them with the user's skills and preferences, resulting in customized job recommendations. By utilizing this system, users can discover relevant job opportunities tailored to their qualifications and interests.

### Chatbot

Interview Ace's chatbot leverages the natural language processing capabilities of ChatGPT. Based on the user's selected job, the chatbot generates interview questions that are specifically tailored to the chosen position. This feature provides a more personalized interview experience and helps users practice answering industry-specific questions.

### Facial Expression Recognition

The facial expression recognition system in Interview Ace plays a crucial role in assessing the user's emotional responses during interview practice. It utilizes a Convolutional Neural Network (CNN) trained on the FER2013 dataset. The FER2013 dataset consists of grayscale images of faces labeled with seven different emotions. By capturing the user's facial expressions through the camera, the system analyzes them using the trained CNN model. Users receive detailed feedback on their emotional responses, along with suggestions for improvement, empowering them to enhance their interview skills.

### Authentication and Interfaces

Interview Ace utilizes the .NET framework for authentication and interface development. The application ensures secure user authentication, protecting sensitive data, and providing a seamless user experience.

## Documentation

You can find the complete documentation of Interview Ace in the [PDF Rapport](./InterviewAce[3372].pdf) file. It provides detailed information about the architecture, implementation, and usage of the application.

## Conclusion

Interview Ace stands as a unique and innovative tool that combines personalized job recommendations, interview practice with tailored questions, and feedback on emotional responses. Its aim is to empower job seekers by equipping them with the necessary tools to succeed in their job search and interview endeavors. Through the content-based recommendation system, facial expression recognition, and the integration of .NET technologies, Interview Ace provides invaluable assistance to users seeking employment.

Please note that the above description is a sample README file for Interview Ace. You can modify and enhance it according to your specific application requirements and additional features.
