import { Description, Dialog, DialogPanel, DialogTitle } from "@headlessui/react";
// import { useState } from "react";

export default function Confirm({ isOpen, setIsOpen, confirmDelete }) {
  const ondeleteClick = () => {
    setIsOpen(false);

    confirmDelete();
  };
  return (
    <>
      <div className={`fixed inset-0 bg-black/60 backdrop-blur-sm z-40 transition-opacity duration-300 opacity-100`} />
      <Dialog open={isOpen} onClose={() => setIsOpen(false)} className="relative z-50 ">
        <div className="fixed inset-0 flex w-screen items-center justify-center p-4">
          <DialogPanel className="max-w-lg space-y-4 border p-12 bg-black/90 rounded-3xl">
            <DialogTitle className="font-bold text-white text-lg">Delete account</DialogTitle>
            <Description className="text-red-700">This will permanently delete your account</Description>
            <p className="text-white">Are you sure you want to delete your account? All of your data will be permanently removed.</p>
            <div className="flex gap-4">
              <button className="btn bg-white/60" onClick={() => setIsOpen(false)}>
                Cancel
              </button>
              <button className="btn btn-danger" onClick={() => ondeleteClick()}>
                Delete
              </button>
            </div>
          </DialogPanel>
        </div>
      </Dialog>
    </>
  );
}
