import "./App.css";
import Landing from "./components/Landing/Landing";
import { Routes, Route } from "react-router-dom";
import Navbar from "./components/Navbars/Navbar";
import Login from "./components/Auth/Login";
import MyProfile from "./components/Profile/MyProfile";
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
import ShowDeveloper from "./components/Developers/ShowDevoloper";
import ChatWithAI from "./components/Chats/ChatWithAI";

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
          <Route index element={<MyProfile />} />
          <Route path="edit" element={<EditAccount />} />
          <Route path="addEducation" element={<AddEducation />} />
          <Route path="addExperience" element={<AddExperience />} />
        </Route>

        {/* Developer */}
        <Route path="/developers" element={<Developers />}></Route>
        <Route path="/showDeveloper/:id" element={<Home />}>
          <Route index element={<ShowDeveloper />} />
        </Route>

        {/* Chats */}
        <Route path="/home" element={<Home />}>
          <Route index path="chat" element={<ChatWithAI />} />
        </Route>
      </Routes>
    </TokenProvider>
  );
}

export default App;
