import "./App.css";
import Landing from "./components/Landing/Landing";
import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbars/Navbar";
import Login from "./components/Auth/Login";
import Profile from "./components/Profile/Profile";
import Home from "./components/Posts/Home";
import Posts from "./components/Posts/Posts";
import Developers from "./components/Developers/Developers";
import Register from "./components/Auth/Register";
import Settings from "./components/Editting/Settings";
import EditAccount from "./components/Editting/EditAccount";
import AddEducation from "./components/Editting/AddEducation";
import AddExperience from "./components/Editting/AddExperience";
import Discussion from "./components/Posts/Discussion";
import { TokenProvider } from "./components/TokenContext";
import Explore from "./components/Posts/Explore.";

function App() {
  return (
    <TokenProvider>
      <Routes>
        {/* Authntication */}
        <Route path="/" element={<Navbar />}>
          <Route index element={<Landing />} />
          <Route path="login" element={<Login />} />
          <Route path="register" element={<Register />} />
        </Route>

        {/* Home */}
        <Route path="/home" element={<Home />}>
          <Route index element={<Posts />} />
          <Route path="discussion/:postId" element={<Discussion />} />
        </Route>

        {/* Explore */}
        <Route path="/explore" element={<Explore />} />

        {/* Settings */}
        <Route path="/settings" element={<Home />}>
          <Route index element={<Settings />} />
        </Route>

        {/* Profile */}
        <Route path="/profile" element={<Home />}>
          <Route index element={<Profile />} />
          <Route path="edit" element={<EditAccount />} />
          <Route path="addEducation" element={<AddEducation />} />
          <Route path="addExperience" element={<AddExperience />} />
        </Route>

        {/* Developer */}
        <Route path="/developers" element={<Developers />}></Route>
      </Routes>
    </TokenProvider>
  );
}

export default App;
