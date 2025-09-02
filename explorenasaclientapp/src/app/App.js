import React from 'react';
import './App.css';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import NavBar, {
  HOME,
} from '../components/NavBar/NavBar';
import HomeWrapper from '../components/layout/HomeWrapper/HomeWrapper';

function App() {
  return (
    <BrowserRouter>
      <NavBar />
      <Routes>
        <Route path="/" element={<HomeWrapper />} />
        <Route path={`/${HOME}`} element={<HomeWrapper />} />
      </Routes>
    </BrowserRouter>
  );
}

export default App;
